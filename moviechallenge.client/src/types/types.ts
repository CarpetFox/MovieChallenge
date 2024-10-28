//these could be split into multiple files, or grouped, etc
export type Movie = { id: number, title: string, overview: string, genres: Genre[], posterUrl: string, originalLanguage: string, popularity: number };
export type Genre = { id: number, name: string };