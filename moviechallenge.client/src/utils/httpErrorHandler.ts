import { EnqueueSnackbar } from "notistack";

export const handleErrorCase = async (response: Response, enqueueSnackbar: EnqueueSnackbar, defaultMessage: string, callback: () => void) => {
    if (response.status === 500) {
        let snackbarMessage = defaultMessage;
        try {
            const data = await response.json();
            if (data.uiFriendlyError) snackbarMessage = data.uiFriendlyError;
        }
        finally {
            enqueueSnackbar(snackbarMessage, { variant: 'error' });
            callback();
        }
    }
}