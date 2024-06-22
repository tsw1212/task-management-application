

namespace DO;

// decleration of the entity dependency
public record Dependency

    (
    //כאשר יוצרים תלות חייב להיות ערכים של המשימות
     int Id=0,
     int DependentTask=0,
     int DependsTask = 0
    )
{
    //defult constructor
    public Dependency():this(0,0,0) { }
}
