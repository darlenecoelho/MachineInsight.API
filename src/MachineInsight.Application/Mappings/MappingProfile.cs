using AutoMapper;
using MachineInsight.Application.DTOs;
using MachineInsight.Domain.Entities;
using MachineInsight.Domain.ValueObjects;

namespace MachineInsight.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Machine, ResponseMachineDto>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.Longitude))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<CreateMachineDto, Machine>()
            .ConstructUsing(dto => new Machine(
                dto.Name,
                new Location(dto.Latitude, dto.Longitude),
                dto.Status
    ));
    }
}