using Streaky.Movies.Validations;

namespace Streaky.Movies.DTOs;

public class ActorCreationDTO : ActorPatchDTO
{
    [FileSizeValidation(maxSizeInMegaBytes: 4)]
    [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
    public IFormFile Photo { get; set; }
}

