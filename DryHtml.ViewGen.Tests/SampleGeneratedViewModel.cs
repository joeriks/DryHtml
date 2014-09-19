﻿using System.Collections.Generic;
public partial class PeopleViewModel
{
    public string Introduction { get; set; }
    public partial class Person
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
    }
    public List<Person> PersonList { get; set; }
}