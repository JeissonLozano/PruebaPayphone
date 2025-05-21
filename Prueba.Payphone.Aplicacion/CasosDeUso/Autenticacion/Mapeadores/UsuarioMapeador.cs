using AutoMapper;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;
using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Autenticacion.Mapeadores;

public class UsuarioMapeador : Profile
{
    public UsuarioMapeador()
    {
        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.NombreUsuario))
            .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.CorreoElectronico))
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
            .ForMember(dest => dest.UltimoAcceso, opt => opt.MapFrom(src => src.UltimoAcceso));
    }
}