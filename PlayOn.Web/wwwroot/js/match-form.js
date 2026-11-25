// Create/Edit match.
//  - courts: show only the courts that support the chosen sport (CourtSport)
//  - venue slot: it holds the date and the start time, so show them and block durations that end after closing time
//  - max players: cannot exceed the capacity of the chosen court
(function () {
    const $ = role => document.querySelector(`[data-role="${role}"]`);
    const sport = $('sport');
    const court = $('court');
    const venue = $('venue');
    const duration = $('duration');
    const maxPlayers = $('max-players');
    const info = document.getElementById('slot-info');

    if (!sport || !court || !venue || !duration || !maxPlayers) return;

    const minutes = hhmm => {
        if (!hhmm || !hhmm.includes(':')) return 0;
        const [h, m] = hhmm.split(':').map(Number);
        return (Number.isFinite(h) ? h : 0) * 60 + (Number.isFinite(m) ? m : 0);
    };

    function filterCourts() {
        const id = sport.value;
        court.querySelectorAll('option[data-sports]').forEach(option => {
            const sports = (option.dataset.sports || '').split(',').filter(Boolean);
            const ok = !id || sports.includes(id);
            option.hidden = !ok;
            option.disabled = !ok;
        });

        if (court.selectedOptions[0]?.disabled) {
            court.value = '';
        }

        limitPlayers();
    }

    function limitPlayers() {
        const capacity = court.selectedOptions[0]?.dataset.capacity;
        if (capacity) {
            const capNum = Number(capacity);
            maxPlayers.max = String(capNum);
            if (Number(maxPlayers.value) > capNum) {
                maxPlayers.value = String(capNum);
            }
        } else {
            maxPlayers.removeAttribute('max');
        }
    }

    function updateSlot() {
        const option = venue.selectedOptions[0];
        if (!option || !option.value) {
            [...duration.options].forEach(o => {
                o.disabled = false;
            });
            if (info) info.textContent = '';
            return;
        }

        const available = minutes(option.dataset.closes) - minutes(option.dataset.starts);
        let best = null;

        [...duration.options].forEach(o => {
            const isTooLong = Number(o.value) > available;
            o.disabled = isTooLong;
            if (!isTooLong) {
                best = o;
            }
        });

        if (duration.selectedOptions[0]?.disabled && best) {
            duration.value = best.value;
        }

        if (info) {
            info.textContent =
                `Starts at ${option.dataset.starts}. The venue is open ${option.dataset.opens} - ` +
                `${option.dataset.closes}, so the match can last up to ${Math.max(0, available)} minutes.`;
        }
    }

    sport.addEventListener('change', filterCourts);
    court.addEventListener('change', limitPlayers);
    venue.addEventListener('change', updateSlot);

    filterCourts();
    updateSlot();
})();