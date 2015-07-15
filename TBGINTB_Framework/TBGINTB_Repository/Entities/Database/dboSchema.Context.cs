﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GinTub.Repository.Entities.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
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
    
    
        public virtual int CreateDefaultPlayerStates(Nullable<System.Guid> player)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateDefaultPlayerStates", playerParameter);
        }
    
        public virtual ObjectResult<LoadNounsForRoom_Result> LoadNounsForRoom(Nullable<System.Guid> player, Nullable<int> room)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadNounsForRoom_Result>("LoadNounsForRoom", playerParameter, roomParameter);
        }
    
        public virtual ObjectResult<LoadParagraphStatesForRoom_Result> LoadParagraphStatesForRoom(Nullable<System.Guid> player, Nullable<int> room)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadParagraphStatesForRoom_Result>("LoadParagraphStatesForRoom", playerParameter, roomParameter);
        }
    
        public virtual ObjectResult<LoadRoomStatesForRoom_Result> LoadRoomStatesForRoom(Nullable<System.Guid> player, Nullable<int> room)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadRoomStatesForRoom_Result>("LoadRoomStatesForRoom", playerParameter, roomParameter);
        }
    
        public virtual ObjectResult<LoadArea_Result> LoadArea(Nullable<int> area)
        {
            var areaParameter = area.HasValue ?
                new ObjectParameter("area", area) :
                new ObjectParameter("area", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadArea_Result>("LoadArea", areaParameter);
        }
    
        public virtual int CreateDefaultPlayerInventories(Nullable<System.Guid> player)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateDefaultPlayerInventories", playerParameter);
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
    
        public virtual ObjectResult<LoadArea_Result> LoadGame(Nullable<System.Guid> player)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadArea_Result>("LoadGame", playerParameter);
        }
    
        public virtual ObjectResult<LoadRoom_Result> LoadRoomId(Nullable<System.Guid> player, Nullable<int> room)
        {
            var playerParameter = player.HasValue ?
                new ObjectParameter("player", player) :
                new ObjectParameter("player", typeof(System.Guid));
    
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadRoom_Result>("LoadRoomId", playerParameter, roomParameter);
        }
    
        public virtual ObjectResult<LoadRoom_Result> LoadRoomXYZ(Nullable<System.Guid> player, Nullable<int> area, Nullable<int> x, Nullable<int> y, Nullable<int> z)
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
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadRoom_Result>("LoadRoomXYZ", playerParameter, areaParameter, xParameter, yParameter, zParameter);
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
    
        public virtual ObjectResult<LoadRoom_Result> PlayerMoveXYZ(Nullable<System.Guid> player, Nullable<int> area, Nullable<int> xDir, Nullable<int> yDir, Nullable<int> zDir)
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
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadRoom_Result>("PlayerMoveXYZ", playerParameter, areaParameter, xDirParameter, yDirParameter, zDirParameter);
        }
    
        public virtual ObjectResult<LoadAllVerbTypes_Result> LoadAllVerbTypes()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadAllVerbTypes_Result>("LoadAllVerbTypes");
        }
    
        public virtual ObjectResult<LoadNounsForParagraphState_Result> LoadNounsForParagraphState(Nullable<int> paragraphState)
        {
            var paragraphStateParameter = paragraphState.HasValue ?
                new ObjectParameter("paragraphState", paragraphState) :
                new ObjectParameter("paragraphState", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadNounsForParagraphState_Result>("LoadNounsForParagraphState", paragraphStateParameter);
        }
    
        public virtual ObjectResult<LoadRoom_Result> LoadRoom(Nullable<int> room)
        {
            var roomParameter = room.HasValue ?
                new ObjectParameter("room", room) :
                new ObjectParameter("room", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadRoom_Result>("LoadRoom", roomParameter);
        }
    
        public virtual ObjectResult<LoadMessage_Result> LoadMessage(Nullable<int> message)
        {
            var messageParameter = message.HasValue ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadMessage_Result>("LoadMessage", messageParameter);
        }
    
        public virtual ObjectResult<LoadMessageChoicesForMessage_Result> LoadMessageChoicesForMessage(Nullable<int> message)
        {
            var messageParameter = message.HasValue ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadMessageChoicesForMessage_Result>("LoadMessageChoicesForMessage", messageParameter);
        }
    
        public virtual ObjectResult<LoadMessageId_Result> LoadMessageId(Nullable<int> message)
        {
            var messageParameter = message.HasValue ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LoadMessageId_Result>("LoadMessageId", messageParameter);
        }
    }
}
