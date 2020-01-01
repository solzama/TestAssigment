using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameEngine
{
    public class GameSettings
    {
        public int GameSettingsId { get; set; }
        
        [Required] 
        [MaxLength(255)]
        public string GameName { get; set; } = default!;
        
        [Required] 
        public int BoardHeight { get; set; } = default!;
        
        [Required] 
        public int BoardWidth { get; set; } = default!;
        
        [MaxLength(255)]
        public string? SaveName { get; set; }
        
        [NotMapped]
        public CellState[,]? StartingBoard { get; set; }
        
        [MaxLength(255)]
        public string? SerializedBoard { get; set; }
    }
}