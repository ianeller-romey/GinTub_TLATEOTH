﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GinTub.Repository.Entities;


namespace GinTub.Repository.Interface
{
    public interface IGinTubRepository
    {
        Guid? PlayerLogin(string userName, string domainName, string domain, string password);

        IEnumerable<VerbType> ReadAllVerbTypes();

        Tuple<Area, Room, IEnumerable<RoomState>, IEnumerable<ParagraphState>> ReadGame(Guid playerId);

        Message ReadMessage(int messageId);

        IEnumerable<Noun> GetNounsForParagraphState(int paragraphStateId);

        IEnumerable<Result> GetActionResults(Guid playerId, int nounId, int verbTypeId);
    }
}
