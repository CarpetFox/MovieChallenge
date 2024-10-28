import { EnqueueSnackbar } from "notistack";

export const handleResponse = async (response: Response, enqueueSnackbar: EnqueueSnackbar, defaultMessage: string, successCallback: () => void, errorCallback: (() => void) | null = null) => {
    if (response.status === 500) {
        let snackbarMessage = defaultMessage;
        try {
            const data = await response.json();
            if (data.uiFriendlyError) snackbarMessage = data.uiFriendlyError;
        }
        finally {
            enqueueSnackbar(snackbarMessage, { variant: 'error' });
            errorCallback?.();
        }
    }
    else {
        successCallback();
    }
}