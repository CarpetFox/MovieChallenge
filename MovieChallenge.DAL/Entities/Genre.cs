﻿namespace MovieChallenge.DAL.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Movie> Movies { get; } = [];
        public List<MovieGenre> MovieGenres { get; set; } = [];
    }
}
