internal class Inpost
{
    private static Inpost inpost;
    private static readonly object _lock = new object();
    private int _nrPaczkomatu;

    private Inpost(int nrPaczkomatu) { _nrPaczkomatu = nrPaczkomatu; }

    public static Inpost GetInpost(int nrPaczkomatu)
    {
        if (inpost == null)
        {
            // powiedz innym watkom ze
            // ten obszar bedzie zajety
            lock (_lock)
            {
                if (inpost == null)
                {
                    inpost = new Inpost(nrPaczkomatu);
                }
            }
        }

        // zawsze ten sam obiekt
        return inpost;
    }

    public int NrPaczkomatu { get { return _nrPaczkomatu; } }
}
