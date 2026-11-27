// Shared helpers: antiforgery token and notification bell.
(function () {
    const token = () =>
        document.querySelector('#antiforgery input[name="__RequestVerificationToken"]')?.value ?? '';

    window.playon = { token };

    const bell = document.getElementById('bell');
    if (!bell) return;

    const count = document.getElementById('bell-count');
    const url = bell.dataset.url;

    if (!url || !count) return;

    let isLoading = false;

    async function load() {
        if (isLoading) return;

        isLoading = true;

        try {
            const response = await fetch(url, {
                headers: {
                    Accept: 'application/json'
                }
            });

            if (!response.ok) return;

            const data = await response.json();
            const unread = Math.max(0, Number(data.unread ?? 0));

            count.textContent = String(unread);
            count.classList.toggle('d-none', unread === 0);
        } catch {
            // Ignore transient network errors.
        } finally {
            isLoading = false;
        }
    }

    load();
    setInterval(load, 30000);
})();