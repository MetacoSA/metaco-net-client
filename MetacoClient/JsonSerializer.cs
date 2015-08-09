using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MetacoClient
{
	internal class JsonSerializer
	{
		private readonly JsonSerializerSettings _settings;

		public JsonSerializer()
		{
			_settings = new JsonSerializerSettings {
				ContractResolver = new NullToEmptyStringResolver(),
			};
			_settings.Converters.Add(new DateTimeToEpochConverter());
		}

		public T Deserialize<T>(string json)
		{
			return typeof(T) == typeof(string)
				? (T)(object)json
				: JsonConvert.DeserializeObject<T>(json, _settings);			
		}

		public string Serialize<T>(T obj)
		{
			return JsonConvert.SerializeObject(obj, _settings);	
		}
	}

	internal class NullToEmptyStringResolver : DefaultContractResolver
	{
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			return type.GetProperties()
					.Select(p =>
					{
						var jp = base.CreateProperty(p, memberSerialization);
						jp.ValueProvider = new NullToEmptyStringValueProvider(p);
						return jp;
					}).ToList();
		}
	}

	internal class NullToEmptyStringValueProvider : IValueProvider
	{
		private readonly PropertyInfo _memberInfo;
		public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
		{
			_memberInfo = memberInfo;
		}

		public object GetValue(object target)
		{
			var result = _memberInfo.GetValue(target);
			if (_memberInfo.PropertyType == typeof(string) && result == null) result = "";
			return result;

		}

		public void SetValue(object target, object value)
		{
			_memberInfo.SetValue(target, value);
		}
	}

    internal class DateTimeToEpochConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime).IsAssignableFrom(objectType) || typeof(DateTimeOffset).IsAssignableFrom(objectType) || typeof(DateTimeOffset?).IsAssignableFrom(objectType);
        }

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(reader.Value == null)
                return null;
			var result = DateTimeExtensions.UnixTimeToDateTime((ulong)(long)reader.Value);
            if (objectType == typeof(DateTime))
                return result.UtcDateTime;
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            DateTime time;
            if (value is DateTime)
                time = (DateTime)value;
            else
                time = ((DateTimeOffset)value).UtcDateTime;

			if (time < DateTimeExtensions.UnixTimeToDateTime(0))
				time = DateTimeExtensions.UnixTimeToDateTime(0).UtcDateTime;
            writer.WriteValue(time.ToEpoch());
        }
    }

	internal static class DateTimeExtensions
	{
		private static readonly DateTimeOffset EpochMinValue = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

		public static long ToEpoch(this DateTimeOffset dt)
		{
			return dt.UtcDateTime.ToEpoch();
		}

		public static long ToEpoch(this DateTime dt)
		{
			dt = dt.ToUniversalTime();
			if (dt < EpochMinValue)
				throw new ArgumentOutOfRangeException("dt", "The supplied datetime can't be expressed in unix timestamp");
			var result = (long) (dt - EpochMinValue).TotalSeconds;
			if (result > UInt32.MaxValue)
				throw new ArgumentOutOfRangeException("dt", "The supplied datetime can't be expressed in unix timestamp");
			return result;
		}

		public static DateTimeOffset UnixTimeToDateTime(ulong timestamp)
		{
			var span = TimeSpan.FromSeconds(timestamp);
			return EpochMinValue + span;
		}

	}
}
