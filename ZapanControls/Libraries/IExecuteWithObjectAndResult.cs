namespace ZapanControls.Libraries
{
    // Source : https://github.com/lbugnion/mvvmlight/blob/1f67dfbcfb776b785727c1505566dc5f7c72a891/GalaSoft.MvvmLight/GalaSoft.MvvmLight%20(PCL)/Helpers/IExecuteWithObjectAndResult.cs

    /// <summary>
    /// This interface is meant for the <see cref="WeakFunc{T}" /> class and can be 
    /// useful if you store multiple WeakFunc{T} instances but don't know in advance
    /// what type T represents.
    /// </summary>
    ////[ClassInfo(typeof(WeakAction))]
    public interface IExecuteWithObjectAndResult
    {
        /// <summary>
        /// Executes a Func and returns the result.
        /// </summary>
        /// <param name="parameter">A parameter passed as an object,
        /// to be casted to the appropriate type.</param>
        /// <returns>The result of the operation.</returns>
        object ExecuteWithObject(object parameter);
    }
}