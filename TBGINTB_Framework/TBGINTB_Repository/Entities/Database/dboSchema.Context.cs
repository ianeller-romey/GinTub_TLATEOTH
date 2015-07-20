﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GinTub.Repository.Entities.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GinTubEntities : DbContext
    {
        public GinTubEntities()
            : base("name=GinTubEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        [DbFunction("GinTubEntities", "f_PlayerHasRequirementsForAction")]
        public virtual IQueryable<f_PlayerHasRequirementsForAction_Result> f_PlayerHasRequirementsForAction(Nullable<System.Guid> player, Nullable<int> action)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var actionParameter = action.HasValue ?
                new ObjectParameter("action", action) :
                new ObjectParameter("action", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<f_PlayerHasRequirementsForAction_Result>("[GinTubEntities].[f_PlayerHasRequirementsForAction](@player, @action)", playerParameter, actionParameter);
        }
    
        public virtual int CreateDefaultPlayerInventories(Nullable<System.Guid> player)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateDefaultPlayerInventories", playerParameter);
        }
    
        public virtual int CreateDefaultPlayerStates(Nullable<System.Guid> player)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateDefaultPlayerStates", playerParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> CreatePlayer(string username, string domainname, string domain, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var domainnameParameter = domainname != null ?
                new ObjectParameter("domainname", domainname) :
                new ObjectParameter("domainname", typeof(string));
    
            var domainParameter = domain != null ?
                new ObjectParameter("domain", domain) :
                new ObjectParameter("domain", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("CreatePlayer", usernameParameter, domainnameParameter, domainParameter, passwordParameter);
        }
    
        public virtual ObjectResult<GetActionResults_Result> GetActionResults(Nullable<System.Guid> player, Nullable<int> noun, Nullable<int> verbType)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var nounParameter = noun.HasValue ?
                new ObjectParameter("noun", noun) :
                new ObjectParameter("noun", typeof(int));
    
            var verbTypeParameter = verbType.HasValue ?
                new ObjectParameter("verbType", verbType) :
                new ObjectParameter("verbType", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetActionResults_Result>("GetActionResults", playerParameter, nounParameter, verbTypeParameter);
        }
    
        public virtual ObjectResult<GetMessageChoiceResults_Result> GetMessageChoiceResults(Nullable<int> messageChoice)
        {
            var messageChoiceParameter = messageChoice.HasValue ?
                new ObjectParameter("messageChoice", messageChoice) :
                new ObjectParameter("messageChoice", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetMessageChoiceResults_Result>("GetMessageChoiceResults", messageChoiceParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> PlayerLogin(string emailUserName, string emailDomainName, string emailDomain, string password)
        {
            var emailUserNameParameter = emailUserName != null ?
                new ObjectParameter("emailUserName", emailUserName) :
                new ObjectParameter("emailUserName", typeof(string));
    
            var emailDomainNameParameter = emailDomainName != null ?
                new ObjectParameter("emailDomainName", emailDomainName) :
                new ObjectParameter("emailDomainName", typeof(string));
    
            var emailDomainParameter = emailDomain != null ?
                new ObjectParameter("emailDomain", emailDomain) :
                new ObjectParameter("emailDomain", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("PlayerLogin", emailUserNameParameter, emailDomainNameParameter, emailDomainParameter, passwordParameter);
        }
    
        public virtual ObjectResult<PlayerMoveXYZ_Result> PlayerMoveXYZ(Nullable<System.Guid> player, Nullable<int> area, Nullable<int> xDir, Nullable<int> yDir, Nullable<int> zDir)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var areaParameter = area.HasValue ?
                new ObjectParameter("area", area) :
                new ObjectParameter("area", typeof(int));
    
            var xDirParameter = xDir.HasValue ?
                new ObjectParameter("xDir", xDir) :
                new ObjectParameter("xDir", typeof(int));
    
            var yDirParameter = yDir.HasValue ?
                new ObjectParameter("yDir", yDir) :
                new ObjectParameter("yDir", typeof(int));
    
            var zDirParameter = zDir.HasValue ?
                new ObjectParameter("zDir", zDir) :
                new ObjectParameter("zDir", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PlayerMoveXYZ_Result>("PlayerMoveXYZ", playerParameter, areaParameter, xDirParameter, yDirParameter, zDirParameter);
        }
    
        public virtual ObjectResult<ReadAllVerbTypes_Result> ReadAllVerbTypes()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadAllVerbTypes_Result>("ReadAllVerbTypes");
        }
    
        public virtual ObjectResult<ReadArea_Result> ReadArea(Nullable<int> area)
        {
            var areaParameter = area.HasValue ?
                new ObjectParameter("area", area) :
                new ObjectParameter("area", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadArea_Result>("ReadArea", areaParameter);
        }
    
        public virtual ObjectResult<ReadArea_Result> ReadAreaForPlayer(Nullable<int> area)
        {
            var areaParameter = area.HasValue ?
                new ObjectParameter("area", area) :
                new ObjectParameter("area", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadArea_Result>("ReadAreaForPlayer", areaParameter);
        }
    
        public virtual ObjectResult<ReadArea_Result> ReadGame(Nullable<System.Guid> player)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadArea_Result>("ReadGame", playerParameter);
        }
    
        public virtual ObjectResult<ReadMessage_Result> ReadMessage(Nullable<int> message)
        {
            var messageParameter = message.HasValue ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadMessage_Result>("ReadMessage", messageParameter);
        }
    
        public virtual ObjectResult<ReadMessageChoicesForMessage_Result> ReadMessageChoicesForMessage(Nullable<int> message)
        {
            var messageParameter = message.HasValue ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadMessageChoicesForMessage_Result>("ReadMessageChoicesForMessage", messageParameter);
        }
    
        public virtual ObjectResult<ReadMessage_Result> ReadMessageForPlayer(Nullable<int> message)
        {
            var messageParameter = message.HasValue ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadMessage_Result>("ReadMessageForPlayer", messageParameter);
        }
    
        public virtual ObjectResult<ReadNounsForParagraphState_Result> ReadNounsForParagraphState(Nullable<int> paragraphState)
        {
            var paragraphStateParameter = paragraphState.HasValue ?
                new ObjectParameter("paragraphState", paragraphState) :
                new ObjectParameter("paragraphState", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadNounsForParagraphState_Result>("ReadNounsForParagraphState", paragraphStateParameter);
        }
    
        public virtual ObjectResult<ReadNounsForPlayerRoom_Result> ReadNounsForPlayerRoom(Nullable<System.Guid> player, Nullable<int> room)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadNounsForPlayerRoom_Result>("ReadNounsForPlayerRoom", playerParameter, roomParameter);
        }
    
        public virtual ObjectResult<ReadParagraphStatesForPlayerRoom_Result> ReadParagraphStatesForPlayerRoom(Nullable<System.Guid> player, Nullable<int> room)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadParagraphStatesForPlayerRoom_Result>("ReadParagraphStatesForPlayerRoom", playerParameter, roomParameter);
        }
    
        public virtual ObjectResult<ReadRoom_Result> ReadRoom(Nullable<int> room)
        {
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadRoom_Result>("ReadRoom", roomParameter);
        }
    
        public virtual ObjectResult<ReadRoom_Result> ReadRoomForPlayer(Nullable<System.Guid> player, Nullable<int> room)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadRoom_Result>("ReadRoomForPlayer", playerParameter, roomParameter);
        }
    
        public virtual ObjectResult<ReadRoom_Result> ReadRoomForPlayerXYZ(Nullable<System.Guid> player, Nullable<int> area, Nullable<int> x, Nullable<int> y, Nullable<int> z)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var areaParameter = area.HasValue ?
                new ObjectParameter("area", area) :
                new ObjectParameter("area", typeof(int));
    
            var xParameter = x.HasValue ?
                new ObjectParameter("x", x) :
                new ObjectParameter("x", typeof(int));
    
            var yParameter = y.HasValue ?
                new ObjectParameter("y", y) :
                new ObjectParameter("y", typeof(int));
    
            var zParameter = z.HasValue ?
                new ObjectParameter("z", z) :
                new ObjectParameter("z", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadRoom_Result>("ReadRoomForPlayerXYZ", playerParameter, areaParameter, xParameter, yParameter, zParameter);
        }
    
        public virtual ObjectResult<ReadRoomStatesForPlayerRoom_Result> ReadRoomStatesForPlayerRoom(Nullable<System.Guid> player, Nullable<int> room)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReadRoomStatesForPlayerRoom_Result>("ReadRoomStatesForPlayerRoom", playerParameter, roomParameter);
        }
    }
}