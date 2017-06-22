using System;
using System.IO;

namespace NetBox.IO
{
   /// <summary>
   /// Works on top of a subportion of a stream, making only a small window visible
   /// </summary>
   class BoundedStream : Stream
   {
      private readonly Stream _parent;
      private readonly long _windowStart;
      private readonly long _windowEnd;

      public BoundedStream(Stream parent, long windowStart, long windowEnd)
      {
         _parent = parent;
         _windowStart = windowStart;
         _windowEnd = windowEnd;
      }


      public override bool CanRead => throw new NotImplementedException();

      public override bool CanSeek => throw new NotImplementedException();

      public override bool CanWrite => throw new NotImplementedException();

      public override long Length => throw new NotImplementedException();

      public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public override void Flush()
      {
         throw new NotImplementedException();
      }

      public override int Read(byte[] buffer, int offset, int count)
      {
         throw new NotImplementedException();
      }

      public override long Seek(long offset, SeekOrigin origin)
      {
         throw new NotImplementedException();
      }

      public override void SetLength(long value)
      {
         throw new NotImplementedException();
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
         throw new NotImplementedException();
      }
   }
}