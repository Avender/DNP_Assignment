using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;

namespace Models {
public class Adult : Person {
    public string JobTitle { get; set; }

    public override string ToString() {
        return JsonSerializer.Serialize(this);
    }

    public void Update(Adult toUpdate) {
        JobTitle = toUpdate.JobTitle;
        base.Update(toUpdate);
    }
    
    }
}
