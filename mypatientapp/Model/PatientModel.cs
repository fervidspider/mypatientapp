using System;
using CommunityToolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;


namespace mypatientapp.Model

{
    public class Patient : ObservableObject
    {

        public string Id { get; set; }
        public string firstname { get; set; } = null!;
        public string middlename { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public DateTime dateofbirth { get; set; }
        public Address address { get; set; } = null!;
        public int height { get; set; }
        public int weight { get; set; }
        public string notes { get; set; } = null!;
        public List<String> mental { get; set; } = null!;
        public List<String> physical { get; set; } = null!;
        public List<String> medication { get; set; } = null!;
        public DateTime? _createdOn { get; set; }
        public DateTime? _lastEdited { get; set; }

    }
}

