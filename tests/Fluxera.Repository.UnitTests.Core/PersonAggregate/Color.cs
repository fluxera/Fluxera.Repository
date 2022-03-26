namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using System.Runtime.CompilerServices;
	using Fluxera.Enumeration;

	public sealed class Color : Enumeration<Color>
	{
		public static readonly Color Red = new Color(0, "FF0000");
		public static readonly Color Green = new Color(1, "00FF00");
		public static readonly Color Blue = new Color(2, "0000FF");
		public static readonly Color White = new Color(3, "FFFFFF");
		public static readonly Color Black = new Color(4, "000000");

		/// <inheritdoc />
		private Color(int value, string hexValue, [CallerMemberName] string name = null!)
			: base(value, name)
		{
			this.HexValue = hexValue;
		}

		public string HexValue { get; }
	}
}
