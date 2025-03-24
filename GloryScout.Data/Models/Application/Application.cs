using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class Application
	{
		public Guid Id { get; set; }
		public string Status { get; set; }
		public string Content { get; set; }
		public DateTime SubmittedAt { get; set; } = DateTime.Now;
		public Guid ScoutId { get; set; }
		public Guid PlayerId { get; set; }

		// Navigation properties
		[ForeignKey("ScoutId")]public virtual Scout Scout { get; set; } 
		[ForeignKey("PlayerId")] public virtual Player Player { get; set; } 
	}
}
