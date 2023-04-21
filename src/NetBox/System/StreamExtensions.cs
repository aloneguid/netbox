namespace System {
    using System.Threading.Tasks;
    using System.Threading;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Text;
    using System.Diagnostics;
    using System.Threading.Tasks.Sources;

    /// <summary>
    /// <see cref="Stream"/> extension
    /// </summary>
    public static class StreamExtensions {
        #region [ General ]

        /// <summary>
        /// Attemps to get the size of this stream by reading the Length property, otherwise returns 0.
        /// </summary>
        public static bool TryGetSize(this Stream s, out long size) {
            try {
                size = s.Length;
                return true;
            } catch(NotSupportedException) {

            } catch(ObjectDisposedException) {

            }

            size = 0;
            return false;
        }

        /// <summary>
        /// Attemps to get the size of this stream by reading the Length property, otherwise returns 0.
        /// </summary>
        public static long? TryGetSize(this Stream s) {
            long size;
            if(TryGetSize(s, out size)) {
                return size;
            }

            return null;
        }

        #endregion

        #region [ Seek and Read ]

        /// <summary>
        /// Reads the stream until a specified sequence of bytes is reached.
        /// </summary>
        /// <returns>Bytes before the stop sequence</returns>
        public static byte[] ReadUntil(this Stream s, byte[] stopSequence) {
            byte[] buf = new byte[1];
            var result = new List<byte>(50);
            int charsMatched = 0;

            while(s.Read(buf, 0, 1) == 1) {
                byte b = buf[0];
                result.Add(b);

                if(b == stopSequence[charsMatched]) {
                    if(++charsMatched == stopSequence.Length) {
                        break;
                    }
                } else {
                    charsMatched = 0;
                }

            }
            return result.ToArray();
        }

        #endregion

        #region [ Stream Conversion ]

        /// <summary>
        /// Reads all stream in memory and returns as byte array
        /// </summary>
        public static byte[]? ToByteArray(this Stream? stream) {
            if(stream == null)
                return null;
            using(var ms = new MemoryStream()) {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converts the stream to string using specified encoding. This is done by reading the stream into
        /// byte array first, then applying specified encoding on top.
        /// </summary>
        public static string? ToString(this Stream? stream, Encoding encoding) {
            if(stream == null)
                return null;
            if(encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            using(StreamReader reader = new StreamReader(stream, encoding)) {
                return reader.ReadToEnd();
            }
        }

        #endregion

        #region [ Polyfills ]

#if NET7_0_OR_GREATER
#else
        private static void ValidateBufferArguments(byte[] buffer, int offset, int count) {
            if(buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            if(offset < 0) {
                throw new ArgumentException("shoudl be >= 0", nameof(offset));
            }

            if((uint)count > buffer.Length - offset) {
                throw new ArgumentException("invalid", nameof(offset));
            }
        }

        private static async ValueTask<int> ReadAtLeastAsyncCore(Stream s, Memory<byte> buffer, int minimumBytes, bool throwOnEndOfStream, CancellationToken cancellationToken) {
            Debug.Assert(minimumBytes <= buffer.Length);

            int totalRead = 0;
            while(totalRead < minimumBytes) {
                int read = await s.ReadAsync(buffer.Slice(totalRead), cancellationToken).ConfigureAwait(false);
                if(read == 0) {
                    if(throwOnEndOfStream) {
                        throw new EndOfStreamException();
                    }

                    return totalRead;
                }

                totalRead += read;
            }

            return totalRead;
        }

        private static ValueTask AsValueTask<T>(this ValueTask<T> valueTask) {
            if(valueTask.IsCompletedSuccessfully) {
                valueTask.GetAwaiter().GetResult();
                return default;
            }

            return new ValueTask(valueTask.AsTask());
        }

        /// <summary>
        /// Asynchronously reads <paramref name="count"/> number of bytes from the current stream, advances the position within the stream,
        /// and monitors cancellation requests.
        /// </summary>
        /// <param name="buffer">The buffer to write the data into.</param>
        /// <param name="offset">The byte offset in <paramref name="buffer"/> at which to begin writing data from the stream.</param>
        /// <param name="count">The number of bytes to be read from the current stream.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is outside the bounds of <paramref name="buffer"/>.
        /// -or-
        /// <paramref name="count"/> is negative.
        /// -or-
        /// The range specified by the combination of <paramref name="offset"/> and <paramref name="count"/> exceeds the
        /// length of <paramref name="buffer"/>.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached before reading <paramref name="count"/> number of bytes.
        /// </exception>
        /// <remarks>
        /// When <paramref name="count"/> is 0 (zero), this read operation will be completed without waiting for available data in the stream.
        /// </remarks>
        public static ValueTask ReadExactlyAsync(this Stream s, byte[] buffer, int offset, int count, CancellationToken cancellationToken = default) {
            ValidateBufferArguments(buffer, offset, count);

            ValueTask<int> vt = ReadAtLeastAsyncCore(s, buffer.AsMemory(offset, count), count, true, cancellationToken);

            return vt.AsValueTask();
        }

#endif

#endregion
    }
}