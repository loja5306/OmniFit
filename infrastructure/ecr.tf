resource "aws_ecr_repository" "backend" {
  name = "omnifit-backend"

  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }
}