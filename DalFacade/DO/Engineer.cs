

namespace DO;

// decleration of the entity engineer
public record Engineer
 (
    int Id,
    string Name ,
    string Email,
    EngineerExperience‏ Level,
    double Cost


 )
{
    //defult constructor
    public Engineer() : this(0,"","", EngineerExperience‏.Novice, 0.1) { }
}


  


