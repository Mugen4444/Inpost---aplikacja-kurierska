Thread t1 = new Thread(() =>
{
    Inpost inpost = Inpost.GetInpost(10);
    Console.WriteLine("Nr paczkomatu: " + inpost.NrPaczkomatu);
});

Thread t2 = new Thread(() =>
{
    Inpost inpost = Inpost.GetInpost(21);
    Console.WriteLine("Nr paczkomatu: " + inpost.NrPaczkomatu);
});

// numery beda takie same
t1.Start();
t2.Start();

t1.Join();
t2.Join();
