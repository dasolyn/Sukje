using System;

namespace BinaryTree {
    class AddressBook : IComparable<AddressBook> {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }

        public int CompareTo(AddressBook other) {
            return Name.CompareTo(other.Name);
        }
    }
}
