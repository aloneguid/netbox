  
namespace NetBox
{
	using System;

	public partial class DynamicValue
	{

		/// <summary>
		/// Implicitly converts to int data type
		/// </summary>
		public static implicit operator int(DynamicValue dv)
		{
			if(dv == null) return default(int);
			return dv.GetValue<int>();
		}

		/// <summary>
		/// Implicitly converts from int to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(int v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to uint data type
		/// </summary>
		public static implicit operator uint(DynamicValue dv)
		{
			if(dv == null) return default(uint);
			return dv.GetValue<uint>();
		}

		/// <summary>
		/// Implicitly converts from uint to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(uint v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to long data type
		/// </summary>
		public static implicit operator long(DynamicValue dv)
		{
			if(dv == null) return default(long);
			return dv.GetValue<long>();
		}

		/// <summary>
		/// Implicitly converts from long to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(long v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to ulong data type
		/// </summary>
		public static implicit operator ulong(DynamicValue dv)
		{
			if(dv == null) return default(ulong);
			return dv.GetValue<ulong>();
		}

		/// <summary>
		/// Implicitly converts from ulong to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(ulong v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to double data type
		/// </summary>
		public static implicit operator double(DynamicValue dv)
		{
			if(dv == null) return default(double);
			return dv.GetValue<double>();
		}

		/// <summary>
		/// Implicitly converts from double to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(double v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to Guid data type
		/// </summary>
		public static implicit operator Guid(DynamicValue dv)
		{
			if(dv == null) return default(Guid);
			return dv.GetValue<Guid>();
		}

		/// <summary>
		/// Implicitly converts from Guid to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(Guid v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to DateTime data type
		/// </summary>
		public static implicit operator DateTime(DynamicValue dv)
		{
			if(dv == null) return default(DateTime);
			return dv.GetValue<DateTime>();
		}

		/// <summary>
		/// Implicitly converts from DateTime to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(DateTime v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to DateTimeOffset data type
		/// </summary>
		public static implicit operator DateTimeOffset(DynamicValue dv)
		{
			if(dv == null) return default(DateTimeOffset);
			return dv.GetValue<DateTimeOffset>();
		}

		/// <summary>
		/// Implicitly converts from DateTimeOffset to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(DateTimeOffset v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to string data type
		/// </summary>
		public static implicit operator string(DynamicValue dv)
		{
			if(dv == null) return default(string);
			return dv.GetValue<string>();
		}

		/// <summary>
		/// Implicitly converts from string to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(string v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to byte[] data type
		/// </summary>
		public static implicit operator byte[](DynamicValue dv)
		{
			if(dv == null) return default(byte[]);
			return dv.GetValue<byte[]>();
		}

		/// <summary>
		/// Implicitly converts from byte[] to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(byte[] v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to bool data type
		/// </summary>
		public static implicit operator bool(DynamicValue dv)
		{
			if(dv == null) return default(bool);
			return dv.GetValue<bool>();
		}

		/// <summary>
		/// Implicitly converts from bool to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(bool v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to byte data type
		/// </summary>
		public static implicit operator byte(DynamicValue dv)
		{
			if(dv == null) return default(byte);
			return dv.GetValue<byte>();
		}

		/// <summary>
		/// Implicitly converts from byte to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(byte v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to sbyte data type
		/// </summary>
		public static implicit operator sbyte(DynamicValue dv)
		{
			if(dv == null) return default(sbyte);
			return dv.GetValue<sbyte>();
		}

		/// <summary>
		/// Implicitly converts from sbyte to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(sbyte v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to char data type
		/// </summary>
		public static implicit operator char(DynamicValue dv)
		{
			if(dv == null) return default(char);
			return dv.GetValue<char>();
		}

		/// <summary>
		/// Implicitly converts from char to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(char v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to decimal data type
		/// </summary>
		public static implicit operator decimal(DynamicValue dv)
		{
			if(dv == null) return default(decimal);
			return dv.GetValue<decimal>();
		}

		/// <summary>
		/// Implicitly converts from decimal to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(decimal v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to float data type
		/// </summary>
		public static implicit operator float(DynamicValue dv)
		{
			if(dv == null) return default(float);
			return dv.GetValue<float>();
		}

		/// <summary>
		/// Implicitly converts from float to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(float v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to short data type
		/// </summary>
		public static implicit operator short(DynamicValue dv)
		{
			if(dv == null) return default(short);
			return dv.GetValue<short>();
		}

		/// <summary>
		/// Implicitly converts from short to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(short v)
		{
			return new DynamicValue(v);
		}


		/// <summary>
		/// Implicitly converts to ushort data type
		/// </summary>
		public static implicit operator ushort(DynamicValue dv)
		{
			if(dv == null) return default(ushort);
			return dv.GetValue<ushort>();
		}

		/// <summary>
		/// Implicitly converts from ushort to an instance of DynamicValue
		/// </summary>
		public static implicit operator DynamicValue(ushort v)
		{
			return new DynamicValue(v);
		}

	}
}