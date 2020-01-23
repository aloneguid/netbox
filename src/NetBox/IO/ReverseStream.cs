using System.IO;

namespace NetBox.IO
{
   class ReverseStream : Stream
   {
      public override void Flush()
      {
         throw new System.NotImplementedException();
         
         
      }

      public override int Read(byte[] buffer, int offset, int count)
      {
         throw new System.NotImplementedException();
      }

      public override long Seek(long offset, SeekOrigin origin)
      {
         throw new System.NotImplementedException();
      }

      public override void SetLength(long value)
      {
         throw new System.NotImplementedException();
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
         throw new System.NotImplementedException();
      }

      public override bool CanRead => true;
      public override bool CanSeek => false;
      public override bool CanWrite => true;
      public override long Length { get; }
      public override long Position { get; set; }
   }
}