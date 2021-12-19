namespace Fluxera.Repository.MongoDB
{
	//internal sealed class TimeOnlyConvention : ConventionBase, IMemberMapConvention
	//{
	//	/// <inheritdoc />
	//	public void Apply(BsonMemberMap memberMap)
	//	{
	//		Type originalMemberType = memberMap.MemberType;
	//		Type memberType = originalMemberType.UnwrapNullableType();

	//		if(memberType == typeof(TimeOnly))
	//		{
	//			DateTimeSerializer dateTimeSerializer = DateTimeSerializer.UtcInstance
	//				.WithRepresentation(BsonType.String)
	//				.WithDateOnly(true);

	//			memberMap.SetSerializer(dateTimeSerializer);

	//			if(originalMemberType.IsNullable())
	//			{
	//				memberMap.SetSerializer(dateTimeSerializer);
	//			}
	//			else
	//			{
	//				memberMap.SetSerializer(new NullableSerializer<DateTime>(dateTimeSerializer));
	//			}
	//		}
	//	}
	//}

	//internal sealed class DateOnlyConvention : ConventionBase, IMemberMapConvention
	//{
	//	/// <inheritdoc />
	//	public void Apply(BsonMemberMap memberMap)
	//	{
	//		Type originalMemberType = memberMap.MemberType;
	//		Type memberType = originalMemberType.UnwrapNullableType();

	//		if(memberType == typeof(DateOnly))
	//		{
	//			DateOnlySerializer dateTimeSerializer = DateOnlySerializer.Instance
	//				.WithRepresentation(BsonType.String)
	//				.WithDateOnly(true);

	//			memberMap.SetSerializer(dateTimeSerializer);

	//			if(originalMemberType.IsNullable())
	//			{
	//				memberMap.SetSerializer(dateTimeSerializer);
	//			}
	//			else
	//			{
	//				memberMap.SetSerializer(new NullableSerializer<DateOnly>(dateTimeSerializer));
	//			}
	//		}
	//	}
	//}

	//internal sealed class DateOnlySerializer : StructSerializerBase<DateOnly>
	//{
	//	private static readonly DateOnlySerializer __localInstance = new DateOnlySerializer();


	//	public static DateOnlySerializer Instance => instance;
	//}
}
