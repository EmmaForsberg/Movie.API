﻿using AutoMapper;
using MovieCore.DTOs;
using MovieCore.Entities;       // för MovieDetails, Review etc.


namespace MovieApi
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Enkel mappning Actor -> ActorDto (här måste vi hantera Role från MovieActor separat)
            CreateMap<Actor, ActorDto>()
                // Role finns inte direkt i Actor, utan i MovieActor, så vi ignorerar den här mappningen direkt.
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            // Om du vill mappa MovieActor till ActorDto (med Role), kan du göra en separat mappning:
            CreateMap<MovieActor, ActorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Actor.Name))
                .ForMember(dest => dest.BirthYear, opt => opt.MapFrom(src => src.Actor.BirthYear))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            // Mappa Genre -> string (GenreName) via Movie -> MovieDto
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name));

            // Mappa MovieDetailsCreateDto -> MovieDetails (vid skapande)
            CreateMap<MovieDetailsCreateDto, MovieDetails>();

            // Mappa MovieCreateDto -> Movie (vid skapande)
            CreateMap<MovieCreateDto, Movie>();

            // Mappa MovieUpdateDto -> Movie (vid uppdatering)
            CreateMap<MovieUpdateDto, Movie>()
             .ForMember(dest => dest.Id, opt => opt.Ignore());


            // Mappa Review -> ReviewDto
            CreateMap<ReviewCreateDto, Review>();
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ReviewerName));

            // Mappa Movie -> MovieDetailDto (för detaljvy)
            CreateMap<Movie, MovieDetailDto>()
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.Synopsis, opt => opt.MapFrom(src => src.MovieDetails.Synopsis))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.MovieDetails.Language))
                .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.MovieDetails.Budget))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.MovieActors));

            // Mappa ActorWithRoleDto till Actor (vid skapande/uppdatering)
            CreateMap<ActorWithRoleDto, Actor>();

            CreateMap<MovieActorCreateDto, MovieActor>();

            // Andra mappningar...
            CreateMap<Genre, GenreDto>();

            // Movie -> MovieUpdateDto (för att läsa från databas och applicera patch på DTO)
            CreateMap<Movie, MovieUpdateDto>()
                .ForMember(dest => dest.MovieDetails, opt => opt.MapFrom(src => src.MovieDetails))
                .ForMember(dest => dest.Actors, opt => opt.Ignore()); // om du inte patchar actors

            // MovieDetails <-> MovieDetailsCreateDto (tvåvägs)
            CreateMap<MovieDetails, MovieDetailsCreateDto>().ReverseMap();

            CreateMap<Movie, MoviePatchDto>().ReverseMap();
            CreateMap<MovieDetails, MovieDetailsPatchDto>().ReverseMap();

        }
    }


}
