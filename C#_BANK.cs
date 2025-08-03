namespace QAL_S1_D1
    class bankaccount {
    public const string bankcode = "BNK0001";
    public readonly datetime createddate;
    private int _accountnum;
    private string _fullname;
    private string _nationalid;
    private string _phonenumber;
    private string _address;
    private decimal _balance;

    public string fullname
    {

        get { return _fullname; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                console.WriteLine("full name cannot be empty")
            }
            else { _fullname = value; }
        }
    }
    public string nationalid
    {
        get { return _nationalid; }
        set
        {
            if (string.IsNullEmpty(value) || value.Length < 14)
            {
                console.WriteLine("national id must be 14 digits ")
            }
            else
            {
                _nationalid = value;
            }
        }
    }
    public string phonenumber
    {
        get { return _phonenumber; }
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length != 11 || !value.StartsWith("01"))
            {
                console.WriteLine("phone number must start with '01' and be 11 digits");
            }
            else { _phonenumber = value; }
        }
    }
    public decimal balance
    {
        get { return _balance; }
        set
        {
            if (value < 0)
            {
                console.WriteLine("balance cannot be negative number");
            }
            else { _balance = value; }
        }
    }
    public string address { get; set; }

    public bankaccount()
    {
        createddate = datetime.now;
        _accountnum = 0;
        fullname = "unknown";
        nationalid = "00000000000000";
        phonenumber = "01000000000";
        address = "unknown";
        balance = 0;
    }
    public bankaccount(string Fullname, string Nationalid, string Phonenumber, string Address, decimal Balance)
    {
        createddate = datetime.now;
        fullname = Fullname;
        nationalid = Nationalid;
        phonenumber = Phonenumber;
        address = Address;
        balance = Balance;
    }
    public bankaccouunt(string Fullname, string Nationalid, string Phonenumber, string Address)
        : this(Fullname, Nationalid, Phonenumber, Address, 0)
    {

    }
    public void showdetails()
    {
        console.WriteLine($"bank code:{bankcode}");
        console.WriteLine($"created date:{createddate}");
        console.WriteLine($"full name :{fullname}");
        console.WriteLine($"national id:{natioanlid}");
        console.WriteLine($"address: {address}");
        console.WriteLine($"balance:{balance}");
    }
    public bool isvalidnational(string nationalid)
    {
        return nationalid != null && natioanlid.length == 14;
    }
    public bool isvalidphone(string phonenumber)
    {
        return phonenumber != null && phonenumber.length == 11 && phonenumber.StartsWith("01");
    }
}


    internal class Program
    {
        static void Main(string[] args)
        {
        bankaccount account1 = new bankaccount("noha", "12345678902547", "01256478423",
            "cairo", 5000);
        account1.showdetails();
        }
    }
}
