using System;
using System.Threading.Tasks;

namespace Monospace.NoThreads.State {


    public interface IAsyncAdapter<T> {
    
        Task<TValue> Call<TValue>(Func<T, TValue> visitor);
        
        Task Call(Action<T> visitor);
    }

}