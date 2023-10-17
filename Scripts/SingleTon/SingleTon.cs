
public class SingleTon<T> where T : new()
{
    private static readonly object _lock = new object();
    private static T m_gInstance = default(T);

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (m_gInstance == null)
                {
                    m_gInstance = new T();
                }
                return m_gInstance;
            }
        }
    }

}
