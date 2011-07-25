using System.Threading.Tasks;

namespace Monospace.NoThreads.AsyncCoroutines {
    internal class Exponentiator2 {
        public static async Task<int> AsyncMethod(int input) {
            return input * input;
        }
    }
}