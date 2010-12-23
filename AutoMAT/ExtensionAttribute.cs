using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
	internal sealed class ExtensionAttribute : Attribute
	{
	}
}
