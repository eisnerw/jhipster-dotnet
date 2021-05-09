using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Jhipster.Domain
{
    [Table("task")]
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public IList<Job> Jobs { get; set; } = new List<Job>();

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var task = obj as Task;
            if (task?.Id == null || task?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, task.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "Task{" +
                    $"ID='{Id}'" +
                    $", Title='{Title}'" +
                    $", Description='{Description}'" +
                    "}";
        }
    }
}
