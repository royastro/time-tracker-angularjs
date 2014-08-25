using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using TimeTracker.DataTransferObjects;
using TimeTracker.Model;
using EntryHeader = TimeTracker.Model.EntryHeader;

namespace TimeTracker.Services
{
    public class EntityMapper
    {
        public void SetupEntityToDataTransferObjectMappings()
        {
            Mapper.CreateMap<Task, dtoTask>();
            Mapper.CreateMap<Entry, dtoEntry>();
            Mapper.CreateMap<EntryHeader, dtoEntryHeader>();
            Mapper.CreateMap<UserProfile, dtoUserProfile>();
            //Mapper.AssertConfigurationIsValid();
        }

        public void SetupDataTranferObjectMappingsToEntity()
        {
            Mapper.CreateMap<dtoTask, Task>();
            Mapper.CreateMap<dtoEntry, Entry>(); 
            Mapper.CreateMap<dtoEntryHeader, EntryHeader>();
            Mapper.CreateMap<dtoUserProfile, UserProfile>();
        }
    }
}
