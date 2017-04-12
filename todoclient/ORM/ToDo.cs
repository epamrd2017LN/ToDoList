﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class ToDo
    {
        /// <summary>
        /// Gets or sets to do identifier.
        /// </summary>
        /// <value>
        /// To do identifier.
        /// </value>
        [Key]
        public int ToDoId { get; set; }


        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this todo-item is completed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this todo-item is completed; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the name (description) of todo-item.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        public string Status { get; set; }
    }
}
