const daysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
const monthsOfYear = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

export const formatIsoDate = (isoDate: string): string => {
    const date = new Date(isoDate);

    const day = daysOfWeek[date.getDay()];
    const month = monthsOfYear[date.getMonth()];
    const dateNumber = date.getDate();
    const year = date.getFullYear();

    const hours = date.getHours();
    const minutes = String(date.getMinutes()).padStart(2, "0");
    const time = `${hours}:${minutes}`;

    return `${day}, ${month} ${dateNumber}, ${year}, ${time}`;
};