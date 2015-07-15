using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TBGINTB_Builder.BuilderControls
{
    public interface IRegisterGinTubEventsOnlyWhenActive
    {
        void SetActiveAndRegisterForGinTubEvents();
        void SetInactiveAndUnregisterFromGinTubEvents();
    }
}
