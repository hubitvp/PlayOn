// Shared helpers: antiforgery token for AJAX calls and the notification bell.
(function () {
    const token = () =>
        document.querySelector('#antiforgery input[name="__RequestVerificationToken"]')?.value ?? '';
    window.playon = { token };

    const bell = document.getElementById('bell');
    if (!bell) return;

    const count = document.getElementById('bell-count');
    const list = document.getElementById('bell-list');
    const url = bell.dataset.url;
    if (!url || !count || !list) return;

    let isLoading = false;

    async function load() {
        if (isLoading) return;
        isLoading = true;

        try {
            const response = await fetch(url, { headers: { Accept: 'application/json' } });
            if (!response.ok) return;

            const data = await response.json(); // { unread, items: [{ title, message, link, isRead }] }
            const unread = Number(data.unread ?? 0);
            const items = Array.isArray(data.items) ? data.items : [];

            count.textContent = String(unread);
            count.classList.toggle('d-none', unread === 0);

            list.replaceChildren(...items.map(n => {
                const li = document.createElement('li');
                const a = document.createElement('a');
                a.className = 'dropdown-item' + (n.isRead ? '' : ' fw-semibold');
                a.href = n.link ?? '#';
                a.textContent = n.title ? `${n.title}: ${n.message ?? ''}` : (n.message ?? '');
                li.append(a);
                return li;
            }));
        } catch {
            // Ignore transient network errors
        } finally {
            isLoading = false;
        }
    }

    load();
    setInterval(load, 30000);

    bell.addEventListener('shown.bs.dropdown', async () => {
        try {
            const response = await fetch('/api/notifications/read', {
                method: 'POST',
                headers: { RequestVerificationToken: token() }
            });
            if (response.ok) {
                await load();
            }
        } catch {
            // Ignore transient network errors
        }
    });
})();