using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace StartValuesAnalyzer.SAXParser;

public class CompressedFileStream : Stream
{
	private FileStream fileStream;

	private GZipStream gzipStream;

	private bool firstRead = true;

	private Encoding encoding;

	public override bool CanRead => gzipStream.CanRead;

	public override bool CanSeek => gzipStream.CanSeek;

	public override bool CanWrite => false;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public override int ReadTimeout
	{
		get
		{
			throw new InvalidOperationException();
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	public override int WriteTimeout
	{
		get
		{
			throw new InvalidOperationException();
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	public CompressedFileStream(string path)
	{
		fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
		encoding = Encoding.GetEncoding(1252);
	}

	public override IAsyncResult BeginWrite(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
	{
		throw new InvalidOperationException();
	}

	public override void Close()
	{
		gzipStream.Close();
		base.Close();
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		throw new InvalidOperationException();
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] array, int offset, int count)
	{
		int num = count / 2;
		byte[] array2 = new byte[num];
		int num2 = gzipStream.Read(array2, 0, num);
		if (num2 > 0)
		{
			if (firstRead)
			{
				int count2 = Math.Min(num2, 256);
				string @string = Encoding.ASCII.GetString(array2, 0, count2);
				int num3 = @string.IndexOf("CodePage=");
				if (num3 > -1)
				{
					num3 += 10;
					int num4 = @string.IndexOf('"', num3);
					string s = @string.Substring(num3, num4 - num3);
					encoding = Encoding.GetEncoding(int.Parse(s));
				}
				firstRead = false;
			}
			byte[] array3 = Encoding.Convert(encoding, Encoding.Unicode, array2, 0, num2);
			num2 = Buffer.ByteLength(array3);
			Buffer.BlockCopy(array3, 0, array, offset, num2 - offset);
		}
		return num2;
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] array, int offset, int count)
	{
		throw new InvalidOperationException();
	}

	public override void WriteByte(byte value)
	{
		throw new InvalidOperationException();
	}
}
