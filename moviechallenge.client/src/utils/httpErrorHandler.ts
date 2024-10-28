import { EnqueueSnackbar } from "notistack";

export const handleErrorCase = async (response: Response, enqueueSnackbar: EnqueueSnackbar, defaultMessage: string, callback: () => void) => {
    if (response.status === 500) {
        const data = await response.json();
        enqueueSnackbar(data.uiFriendlyError || defaultMessage, { variant: 'error' });
        callback();
    }
}