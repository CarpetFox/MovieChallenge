/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useRef, useState } from 'react';
import './App.css';
import { Button, CircularProgress, FormControl, InputLabel, MenuItem, Paper, Select, SelectChangeEvent, TextField } from '@mui/material';
import { useSnackbar } from 'notistack';
import FilterAltOffIcon from '@mui/icons-material/FilterAltOff';
import { Genre } from './types/types';
import { debounce } from './utils/debounce';
import MovieTable from './components/MovieTable';

function App() {
    const [genres, setGenres] = useState<Genre[]>();
    const [selectedGenres, setSelectedGenres] = useState<Genre[]>([]);
    const [titleSearch, setTitleSearch] = useState<string>();
    const [dataLoading, setDataLoading] = useState(false);
    const { enqueueSnackbar } = useSnackbar();
    const formRef = useRef<HTMLFormElement>(null);

    const handleChange = (event: SelectChangeEvent<typeof selectedGenres>) => {
        const value = event.target.value;
        setSelectedGenres(typeof value === 'string' ? [] : value);
    };

    const searchChanged = debounce((event: React.ChangeEvent<HTMLInputElement>) => {
        setTitleSearch(event.target.value);
    });

    const handleResetFilters = () => {
        setTitleSearch('');
        setSelectedGenres([]);
        formRef.current?.reset();
    };

    useEffect(() => {
        const loadGenres = async () => {
            setDataLoading(true);

            await fetch('movie/getGenres').then(async result => {
                if (result.status === 500) {
                    enqueueSnackbar('There was an error loading the Genres.', { variant: 'error' });
                    setDataLoading(false);
                    return;
                }

                setGenres(await result.json());
                setDataLoading(false);
            });
        };

        loadGenres();
    }, []);

    return (
        <>
            <Paper sx={{ m: '2rem', padding: '1rem', fontSize: '40px' }}>
                Movies
            </Paper>
            <Paper sx={{ m: '2rem', width: 'calc(100vw - 4rem)' }}>
                <form ref={formRef}>
                    <div style={{ display: 'flex', flexDirection: 'row', padding: '1rem', gap: '1rem' }}>
                        <TextField
                            label="Search by title"
                            variant="outlined"
                            onChange={searchChanged} />

                        <FormControl sx={{ width: 300 }}>
                            <InputLabel id="genres-label">Genres</InputLabel>
                            <Select
                                labelId="genres-label"
                                label="Label"
                                id="demo-simple-select"
                                multiple
                                value={selectedGenres}
                                onChange={handleChange}
                            >
                                {genres?.map((g) => (
                                    <MenuItem key={g.id} value={g.id}>{g.name}</MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                        <Button
                            onClick={handleResetFilters}
                            disabled={!titleSearch && !selectedGenres[0]}
                            variant="outlined"
                            startIcon={<FilterAltOffIcon />}>
                            Reset Filters
                        </Button>
                        {dataLoading && (<CircularProgress sx={{ mt: 1 }} />)}
                    </div>
                </form>

                <MovieTable
                    selectedGenres={selectedGenres}
                    titleSearch={titleSearch || ''}
                />
            </Paper>
        </>
    );
}

export default App;