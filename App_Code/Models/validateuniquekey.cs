using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class validateuniquekey
{
    [Required]
    public string uniquekey { get; set; }
}

public class uniquekeys
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<uniquekey> data { get; set; }
}

public class uniquekey
{
    public string slno { get; set; }
}