using System.Collections.Generic;

namespace AutoWrapper.CodeGen
{
	public sealed class CodeGeneratorPragma
	{
		internal CodeGeneratorPragma() { }

		public CodeGeneratorPragmaWarning Warning { get; } = new CodeGeneratorPragmaWarning();

		public sealed class CodeGeneratorPragmaWarning
		{
			internal CodeGeneratorPragmaWarning() { }

			private List<int> _disabled = new List<int>();

			public void Disable(int warningNumber)
			{
				if (_disabled.Contains(warningNumber) == false)
					_disabled.Add(warningNumber);
			}

			public override string ToString()
			{
				var pragma = "#pragma warning disable ";

				_disabled.ForEach(w => pragma += w + ", ");

				return pragma.TrimEnd(' ', ',');
			}
		}
	}
}
