using Microsoft.AspNetCore.Mvc;
using Streaky.Movies.Helper;
using Streaky.Movies.Validations;

namespace Streaky.Movies.DTOs;

public class MovieCreationDTO : MoviePatchDTO
{
    [FileSizeValidation(maxSizeInMegaBytes: 4)]
    [TypeFileValidation(GroupTypeFile.Image)]
    public IFormFile Poster { get; set; }

    [ModelBinder(binderType: typeof(TypeBinder<List<int>>))]
    public List<int> GendersIds { get; set; }

    [ModelBinder(binderType: typeof(TypeBinder<List<ActorMoviesCreationDTO>>))]
    public List<ActorMoviesCreationDTO> Actors { get; set; }
}

