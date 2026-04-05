import { test, expect } from "@playwright/test";

test("Clicking Exercises link navigates to /exercises", async ({ page }) => {
  await page.goto("/");
  await page.getByRole("link", { name: "Exercises" }).click();
  await expect(page).toHaveURL("/exercises");
});

test("Clicking Workouts link navigates to /workouts", async ({ page }) => {
  await page.goto("/");
  await page.getByRole("link", { name: "Workouts" }).click();
  await expect(page).toHaveURL("/workouts");
});

test("Clicking OmniFit link from exercises returns home", async ({ page }) => {
  await page.goto("/exercises");
  await page.getByRole("link", { name: "OmniFit" }).click();
  await expect(page).toHaveURL("/");
});

test("Clicking OmniFit link from workouts returns home", async ({ page }) => {
  await page.goto("/workouts");
  await page.getByRole("link", { name: "OmniFit" }).click();
  await expect(page).toHaveURL("/");
});
