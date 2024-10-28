import { Button, Dialog, DialogContent, DialogTitle, TextField } from "@mui/material";
import { Movie } from "../types/types";

export interface MovieDialogProps {
    open: boolean;
    movie: Movie | undefined;
    onClose: () => void;
}

function MovieDialog(props: MovieDialogProps) {
    const { open, movie, onClose } = props;

    const handleClose = () => {
        onClose();
    };

    return (
        <Dialog open={open} onClose={handleClose} maxWidth={"lg"}>
            {
                movie &&
                <>
                    <DialogTitle>{movie.title}</DialogTitle>
                    <DialogContent sx={{ padding: '1rem', display: 'flex', flexDirection: 'column' }}>
                        <div style={{ display: 'flex', gap: '1rem', alignItems: 'start', flexWrap: 'wrap' }}>
                            <img src={movie.posterUrl} style={{ width: 280.5, borderRadius: '5px', objectFit: 'contain', flexGrow: 1 }} />
                            <div style={{ display: 'flex', flexDirection: 'column', width: 300, flexBasis: 170, flexGrow: 100 }}>
                                <TextField
                                    sx={{ mt: 1 }}
                                    label="Original Language"
                                    variant="outlined"
                                    inputProps={{ readOnly: true }}
                                    value={movie.originalLanguage} />
                                <TextField
                                    sx={{ mt: 2, mb: 1 }}
                                    label="Popularity"
                                    variant="outlined"
                                    inputProps={{ readOnly: true }}
                                    value={movie.popularity} />
                                <TextField
                                    sx={{ mt: 2 }}
                                    label="Overview"
                                    variant="outlined"
                                    multiline
                                    rows={10}
                                    inputProps={{ readOnly: true }}
                                    value={movie.overview} />
                            </div>
                        </div>
                        <Button
                            sx={{ mt: '1rem', flexGrow: 1 }}
                            onClick={handleClose}
                            variant="outlined">
                            Close
                        </Button>
                    </DialogContent>
                </>
            }
        </Dialog>
    );
}

export default MovieDialog;