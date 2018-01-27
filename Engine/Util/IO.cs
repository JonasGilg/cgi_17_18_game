using System;
using System.Threading.Tasks;

namespace Engine.Util {
	public static class IO {
		public static void PrintAsync(string s) => Task.Run(() => Console.WriteLine(s));
	}
}