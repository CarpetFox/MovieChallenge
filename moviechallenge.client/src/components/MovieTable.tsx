/* eslint-disable react-hooks/exhaustive-deps */
import { useCallback, useEffect, useState } from 'react';
import { DataGrid, GridSortModel } from '@mui/x-data-grid';
import { useSnackbar } from 'notistack';
import moment from 'moment';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { Button } from '@mui/material';
import { Genre, Movie } from '../types/types';
import MovieDialog from './MovieDialog';
import { handleResponse } from '../utils/handleResponse';

function MovieTable({ selectedGenres, titleSearch }: { selectedGenres: Genre[], titleSearch: string }) {
    const [movies, setMovies] = useState<Movie[]>();
    const [paginationModel, setPaginationModel] = useState({
        pageSize: 10,
        page: 0,
    });
    const [sortModel, setSortModel] = useState<{ orderBy: string, orderAscending: boolean }>();
    const [dataLoading, setDataLoading] = useState(false);
    const [rowCount, setRowCount] = useState(0);
    const { enqueueSnackbar } = useSnackbar();
    const [dialogOpen, setDialogOpen] = useState(false);
    const [viewingMovie, setViewingMovie] = useState<Movie>();

    const handleSortModelChange = useCallback((sortModel: GridSortModel) => {
        if (!sortModel[0]) {
            setSortModel({ orderBy: '', orderAscending: true });
            return;
        }
        setSortModel({ orderBy: sortModel[0].field, orderAscending: sortModel[0].sort === 'asc' })
    }, []);

    const handleViewMovieClick = useCallback((movie: Movie) => {
        setDialogOpen(true);
        setViewingMovie(movie);
    }, []);

    useEffect(() => {
        const fetchData = async () => {
            setDataLoading(true);
            let query = `movie?pageSize=${paginationModel.pageSize}&page=${paginationModel.page}&searchModel.title=${titleSearch || ''}&orderBy=${sortModel?.orderBy || 'Id'}&orderAscending=${sortModel?.orderAscending ?? true}`;
            if (selectedGenres && selectedGenres.length) {
                for (const sg of selectedGenres) {
                    query += `&searchModel.genres=${sg}`;
                }
            }
            await fetch(query)
                .then(async response => {
                    await handleResponse(response, enqueueSnackbar, 'There was an error loading the Movie data.', async () => {
                        const data = await response.json();
                        setMovies(data.data);
                        setRowCount(data.total);
                        return;
                    }, () => {
                        setMovies([]);
                        setRowCount(0);
                    });
                    setDataLoading(false);
                })
        };
        fetchData();
    }, [paginationModel, sortModel]);

    useEffect(() => {
        setPaginationModel({ ...paginationModel, page: 0 });
    }, [selectedGenres, titleSearch])

    return (
        <>
            <DataGrid
                rows={movies}
                rowCount={rowCount}
                columns={[
                    { field: 'title', headerName: 'Title', flex: 1 },
                    { field: 'releaseDate', headerName: 'Release Date', width: 135, valueFormatter: (params: string) => moment(params).format("Do MMM YYYY") },
                    { field: 'genres', headerName: 'Genres', flex: 1, sortable: false, valueFormatter: (params: { name: string }[]) => params.map(p => p.name).join(', ') },
                    { field: 'overview', headerName: 'Overview', flex: 2, sortable: false },
                    {
                        field: 'actions', type: 'actions', headerName: 'Actions', width: 120, getActions: (data) => {
                            return [<Button
                                onClick={() => handleViewMovieClick(data.row)}
                                variant="outlined"
                                startIcon={<VisibilityIcon />}>
                                View
                            </Button>]
                        }
                    }
                ]}
                onRowClick={(rows) => { console.log(rows.id) }}
                initialState={{ pagination: { paginationModel } }}
                sx={{ border: 0, maxHeight: 'calc(100vh - 290px)' }}
                paginationMode="server"
                paginationModel={paginationModel}
                pageSizeOptions={[10, 15, 20]}
                onPaginationModelChange={setPaginationModel}
                sortingMode="server"
                onSortModelChange={handleSortModelChange}
                disableColumnFilter
                disableColumnMenu
                loading={dataLoading}
                slotProps={{
                    pagination: {
                        showFirstButton: true,
                        showLastButton: true,
                    },
                }}
            />
            <MovieDialog open={dialogOpen} onClose={() => setDialogOpen(false)} movie={viewingMovie} />
        </>
    );
}

export default MovieTable;