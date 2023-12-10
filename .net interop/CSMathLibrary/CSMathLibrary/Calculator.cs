using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSMathLibrary {
	[Guid("7BFBBF80-A0CB-465D-AD4A-585809E4770B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICalculator {
		int Multiply(int a, int b);
	}

	[Guid("4C205B6C-39A4-403A-85B7-B76D17C1D805")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Calculator : ICalculator {
		public int Multiply(int a, int b) {
			return a * b;
		}
	}
}
