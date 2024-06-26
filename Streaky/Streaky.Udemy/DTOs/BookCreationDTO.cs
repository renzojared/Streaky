﻿using Streaky.Udemy.Validator;
using System.ComponentModel.DataAnnotations;

namespace Streaky.Udemy.DTOs;

public class BookCreationDTO
{
    [FirstCapitalLetter]
    [StringLength(maximumLength: 100)]
    [Required]
    public string Tittle { get; set; }
    public DateTime PublicationDate { get; set; }
    public List<int> AuthorsId { get; set; }
}

